import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NewComponentComponent } from "./components/new-component/new-component.component";
import { CartComponent } from "./components/cart/cart.component";
import { ProductListComponent } from "./components/product-list/product-list.component";
import { CustomerDetailsComponent } from "./components/customer-details/customer-details.component";
import { Product } from "./components/product/product.component";
import { Products } from './components/products/products.component';
import { ReceipeComponent } from './components/receipe/receipe.component';
import { ReceipesComponent } from './components/receipes/receipes.component';
import { User } from './models/User';
import { AuthService } from './service/AuthService';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [LoginComponent, ProfileComponent],
  templateUrl: './app.component.html'
})
export class AppComponent {
  user: User | null = null;

  constructor(private auth: AuthService) {
    this.user = this.auth.getLoggedInUser();
  }

  handleLogin(user: User) {
    this.user = user;
  }

  handleLogout (){
    this.user = null;
  }
}