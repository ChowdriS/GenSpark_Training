import { Component, signal } from '@angular/core';
import { ReceipeService } from '../../services/receipe-service';
import { CommonModule } from '@angular/common';
import { ReceipeDetail } from "../receipe-detail/receipe-detail";

@Component({
  selector: 'app-receipe-list',
  imports: [CommonModule, ReceipeDetail],
  templateUrl: './receipe-list.html',
  styleUrl: './receipe-list.css'
})
export class ReceipeList {
  recipes = signal<any[]>([]);
  loading = signal<boolean>(true)

  constructor(private recipeService: ReceipeService) {}

  ngOnInit(): void {
    this.recipeService.getAllReceipe().subscribe({
      next:(res:any)=>{
        // console.log(res);
        this.recipes.set(res.recipes);
        // console.log(this.recipes());
        this.loading.set(false);
      },
      error:(err:any)=>{
        console.log(err);
      }
    });
  }
}
