import { httpResource } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toSignal, toObservable } from '@angular/core/rxjs-interop';
import { debounceTime } from 'rxjs';
import { z as zod } from 'zod';
import { invalidateCacheByUrlFragment } from '../shared/http-cache';
import { toast } from 'ngx-sonner';

const SingleProductSchema = zod.object({
  id: zod.number(),
  name: zod.string(),
  price: zod.number(),
});
type Product = zod.infer<typeof SingleProductSchema>;

const ProductArraySchema = zod.array(SingleProductSchema);
export type ProductResponse = zod.infer<typeof ProductArraySchema>;


@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly apiUrl = 'http://localhost:5034/api/Product';

  productInput = signal<Partial<Product> | null>(null);
  selectedProductId = signal<number | null>(null);

  query = signal('');
  debouncedQuery = toSignal(
    toObservable(this.query).pipe(debounceTime(600)),
    { initialValue: '' }
  );

  page = signal(1);
  size = signal(10);

  productResource = httpResource(() => {
    return {
      url: this.apiUrl,
      method: 'GET',
      params: {
        nombre: this.debouncedQuery(),
        page: this.page(),
        size: this.size(),
      }
    };
  },
    {
      parse: (data) => {
        const parsedData = ProductArraySchema.parse(data);
        return parsedData;
      },
    }
  );

  addProduct = httpResource(() => {
    const input = this.productInput();
    if (!input) return undefined;

    return {
      url: this.apiUrl,
      method: 'POST',
      body: input,
      headers: {
        'Content-Type': 'application/json',
      },
    };
  },
    {
      parse: (data) => {
        const parsedData = SingleProductSchema.parse(data);
        invalidateCacheByUrlFragment('/api/Product');
        this.productResource.update((products) => {
          if (!products) return products;
          return [parsedData, ...products];
        });
        toast.success('Producto agregado exitosamente');
        this.productInput.set(null);
        return parsedData;
      },
    }
  );

  // updateProduct = httpResource(() => {
  //   const input = this.productInput();
  //   if (!input?.id) return undefined; // solo hacer PUT si el producto tiene ID

  //   return {
  //     url: `${this.apiUrl}/${input.id}`,
  //     method: 'PUT',
  //     body: input,
  //     headers: {
  //       'Content-Type': 'application/json',
  //     },
  //   };
  // }, {
  //   parse: (data) => {
  //     const updated = SingleProductSchema.parse(data);
  //     invalidateCacheByUrlFragment('/api/Product');

  //     this.productResource.update((products) => {
  //       if (!products) return products;
  //       return products.map(p => p.id === updated.id ? updated : p) ?? [];
  //     });
  //     toast.success('Producto actualizado exitosamente');
  //     this.productInput.set(null);
  //     return updated;
  //   }
  // });

  deleteProoduct = httpResource(
    () => {
      const id = this.selectedProductId();
      if (!id) return undefined;

      return {
        url: `${this.apiUrl}/delete/${id}`,
        method: 'DELETE',
        headers: {
          accept: 'application/json',
        },
      };
    },
    {
      parse: () => {
        const inputId = this.selectedProductId();
        invalidateCacheByUrlFragment('/api/Product');
        this.productResource.update((products) => {
          if (!products) return products;
          return products.filter((p) => p.id !== inputId);
        });
        this.productInput.set(null);
        toast.success('Producto eliminado exitosamente');
        return null;
      },
    }
  );

}
