import { HttpErrorResponse, httpResource } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { z as zod } from 'zod';

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
  private readonly apiUrl = 'http://localhost:5255/api/Product';

  productInput = signal<Partial<Product> | null>(null);

  productResource = httpResource(() => {
    return {
      url: this.apiUrl,
      method: 'GET',
    };
  },
    {
      parse: (data) => {
        const parsedData = ProductArraySchema.parse(data);
        return parsedData;
      },
    }
  );

  productValue = computed(() => this.productResource.value() ?? []);
  productError = computed(() => this.productResource.error() as HttpErrorResponse);
  productStatus = computed(() => this.productResource.status());
  productLoading = computed(() => this.productResource.isLoading());


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
        this.productResource.update((products) => {
          if (!products) return products;
          return [parsedData, ...products];
        });
        this.productInput.set(null);
        return parsedData;
      },
    }
  );

  deleteProoduct = httpResource(
    () => {
      const input = this.productInput();
      if (!input?.id) return undefined;

      return {
        url: `${this.apiUrl}/delete/${input.id}`,
        method: 'DELETE',
        headers: {
          accept: 'application/json',
        },
      };
    },
    {
      parse: () => {
        const input = this.productInput();
        if (input?.id != null) {
          this.productResource.update((products) => {
            if (!products) return products;
            return products.filter((p) => p.id !== input.id);
          });
          this.productInput.set(null);
        }
        return null;
      },
    }
  );

}
