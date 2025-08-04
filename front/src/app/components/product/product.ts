import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-product',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product implements OnInit {

  productDetail!: FormGroup;

  readonly productService = inject(ProductService);
  readonly formBuilder = inject(FormBuilder);

  ngOnInit(): void {
    this.productDetail = this.formBuilder.group({
      id: [0],
      name: [''],
      price: [0],
    });
  }

  products = computed(() => this.productService.productResource.value() ?? []);
  isLoading = computed(() => this.productService.productResource.isLoading());
  status = computed(() => this.productService.productResource.status());
  errorMessage = computed(() => this.productService.productResource.error() as HttpErrorResponse);
  page = this.productService.page;
  query = this.productService.query;

  saveProduct() {
    const { id, name, price } = this.productDetail.value;
    if (!name || !price) return;
    const payload = { id, name, price };
    this.productService.productInput.set(payload); // Esto dispara automáticamente add o update
    this.productDetail.reset();
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

  editProduct(id: number) {
    const product = this.products().find(p => p.id === id);
    if (product) {
      this.productDetail.setValue({
        id: product.id,
        name: product.name,
        price: product.price
      });
    }
  }

  deleteProduct(id: number) {
    this.productService.selectedProductId.set(id)
  }

}
