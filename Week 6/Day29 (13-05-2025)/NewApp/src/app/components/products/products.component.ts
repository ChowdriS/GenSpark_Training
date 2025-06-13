import { Component, OnInit } from '@angular/core';
import { ProductModel } from '../../models/product';
import { ProductService } from '../../service/productService';
import { Product } from "../product/product.component";

@Component({
  selector: 'app-products',
  imports: [Product],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})

export class Products implements OnInit {
  products:ProductModel[]|undefined=undefined;
  constructor(private productService:ProductService){

  }
  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(
      {
        next:(data:any)=>{
         this.products = data.products as ProductModel[];
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
  }

}
