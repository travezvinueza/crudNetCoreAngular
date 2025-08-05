import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { HttpErrorResponse } from '@angular/common/http';
import { toast } from 'ngx-sonner';
declare const bootstrap: any;

@Component({
  selector: 'app-product',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
  isEditing = false;

  readonly productService = inject(ProductService);
  readonly formBuilder = inject(FormBuilder);

  productDetailForm = this.formBuilder.group({
    id: [0],
    name: ['', [Validators.required, Validators.minLength(3)]],
    price: [0.01, [Validators.required, Validators.min(0.01)]]
  });


  products = computed(() => this.productService.productResource.value() ?? []);
  isLoading = computed(() => this.productService.productResource.isLoading());
  status = computed(() => this.productService.productResource.status());
  errorMessage = computed(() => this.productService.productResource.error() as HttpErrorResponse);
  page = this.productService.page;
  query = this.productService.query;

  saveProduct() {
    const { id, name, price } = this.productDetailForm.value;
    if (!name || !price || price <= 0) {
      toast.info('El nombre y el precio son obligatorios y deben ser válidos');
      return;
    }
    const payload = { id: id ?? undefined, name, price };
    this.productService.productInput.set(payload); // Esto dispara automáticamente add o update
    console.log('Guardando producto:', payload);
    this.productDetailForm.reset({ id: 0, name: '', price: 0 });
  }

  // Método para actualizar la query (nombre)
  onSearch(event: Event) {
    const input = event.target as HTMLInputElement;
    this.query.set(input.value);
    this.page.set(1);
  }

  changePage(event: Event) {
    const select = event.target as HTMLSelectElement;
    this.page.set(Number(select.value));
  }

  editProduct(productId: number) {
    this.isEditing = true;
    const product = this.products().find(p => p.id === productId);
    if (product) {
      this.productDetailForm.setValue({
        id: product.id,
        name: product.name,
        price: product.price
      });
      const modal = new bootstrap.Modal(document.getElementById('productModal'));
      modal.show();
    }
  }

  deleteProduct(productId: number) {
    const product = this.products().find(p => p.id === productId);
    if (!product) return;

    toast(
      `¿Estás seguro de eliminar el producto "${product.name}"?`,
      {
        action: {
          label: 'Eliminar',
          onClick: () => {
            this.productService.selectedProductId.set(productId);
            toast.success('Producto eliminado correctamente');
          }
        },
        cancel: {
          label: 'Cancelar',
          onClick: () => console.log('Cancel!'),
        },
        duration: 8000,
      }
    );
  }

  openAddModal() {
    this.isEditing = false;
    this.productDetailForm.reset({ id: 0, name: '', price: 0.01 });

    const modal = new bootstrap.Modal(document.getElementById('productModal')!);
    modal.show();
  }

}
