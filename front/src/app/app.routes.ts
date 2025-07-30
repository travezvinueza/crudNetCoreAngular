import { Routes } from '@angular/router';
import { Product } from './components/product/product';

export const routes: Routes = [
    
    { path: 'Product', component: Product },
    { path: "", redirectTo: "/Product", pathMatch: 'full' },
];
