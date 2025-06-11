import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../../service/receipeService';
import { ReceipeComponent } from '../receipe/receipe.component';

@Component({
  selector: 'app-receipes',
  imports: [ReceipeComponent],
  templateUrl: './receipes.component.html',
  styleUrl: './receipes.component.css'
})
export class ReceipesComponent implements OnInit {
  recipes: any[] = [];

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getRecipes().subscribe({
      next: (res:any) => (this.recipes = res.recipes || [], console.log(this.recipes)),
      error: () => this.recipes = []
    });
  }
}
