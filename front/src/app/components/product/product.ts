import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';


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
      price: [''],
    });
  }

  products = this.productService.productValue;
  isLoading = this.productService.productLoading;
  status = this.productService.productStatus;
  errorMessage = this.productService.productError;
  page = this.productService.page;
  query = this.productService.query;

  newProduct() {
    const { name, price } = this.productDetail.value;
    if (!name || !price) {
      return;
    }
    this.productService.productInput.set({ name, price });
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

  deleteProduct(id: number) {
    this.productService.productInput.set({ id })
  }

}
