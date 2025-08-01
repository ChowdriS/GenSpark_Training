import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { Category } from './components/category/category';
import { ProductList } from './components/product-list/product-list';
import { ProductDetail } from './components/product-detail/product-detail';
import { OrderList } from './components/order-list/order-list';
import { ContactUs } from './components/contact-us/contact-us';
import { ColorList } from './components/color-list/color-list';
import { ColorCreate } from './components/color-create/color-create';
import { NewsList } from './components/news-list/news-list';
import { ColorEdit } from './components/color-edit/color-edit';
import { CategoryProducts } from './components/category-products/category-products';
import { CategoryEdit } from './components/category-edit/category-edit';
import { CategoryCreate } from './components/category-create/category-create';
import { CartList } from './components/cart-list/cart-list';
import { CheckoutForm } from './components/checkout-form/checkout-form';
import { NewsMangement } from './components/news-mangement/news-mangement';
import { ProductMangement } from './components/product-mangement/product-mangement';

export const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'home', component: Home ,children:[
        {path: '', component:ProductList},
        {path: 'product/management', component:ProductMangement},
        {path: 'product/:id', component:ProductDetail},
        {path: 'orders', component:OrderList},
        {path: 'contact-us', component:ContactUs},
        {path: 'color', component:ColorList},
        {path: 'news', component:NewsList},
        {path: 'color/create', component:ColorCreate},
        {path: 'color/edit/:id', component:ColorEdit},
        {path: 'category/products/:id', component:CategoryProducts},
        {path: 'category', component:Category},
        {path: 'category/create', component:CategoryCreate},
        {path: 'category/edit/:id', component:CategoryEdit},
        {path: 'cart', component:CartList},
        {path: 'cart/checkout', component:CheckoutForm},
        {path: 'newsmanagement', component:NewsMangement}
    ]},
];

