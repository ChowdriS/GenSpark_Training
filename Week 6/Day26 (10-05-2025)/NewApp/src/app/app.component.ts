import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NewComponentComponent } from "./components/new-component/new-component.component";
import { CartComponent } from "./components/cart/cart.component";
import { ProductListComponent } from "./components/product-list/product-list.component";
import { CustomerDetailsComponent } from "./components/customer-details/customer-details.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CartComponent, ProductListComponent, CustomerDetailsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  cart: { [key: string]: { name: string, count: number } } = {};

  incrementCartCount(product:any){
    const key = product.name;
    if (this.cart[key]) {
      this.cart[key].count += 1;
    } else {
      this.cart[key] = { name: product.name, count: 1 };
  }
  }
}
