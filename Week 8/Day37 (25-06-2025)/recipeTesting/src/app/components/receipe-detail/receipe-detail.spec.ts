import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceipeDetail } from './receipe-detail';

describe('ReceipeDetail', () => {
  let component: ReceipeDetail;
  let fixture: ComponentFixture<ReceipeDetail>;

  const mockRecipe = 
      {
      "id": 1,
      "name": "Classic Margherita Pizza",
      "ingredients": [
        "Pizza dough",
        "Tomato sauce",
        "Fresh mozzarella cheese",
        "Fresh basil leaves",
        "Olive oil",
        "Salt and pepper to taste"
      ],
      "instructions": [
        "Preheat the oven to 475°F (245°C).",
        "Roll out the pizza dough and spread tomato sauce evenly.",
        "Top with slices of fresh mozzarella and fresh basil leaves.",
        "Drizzle with olive oil and season with salt and pepper.",
        "Bake in the preheated oven for 12-15 minutes or until the crust is golden brown.",
        "Slice and serve hot."
      ],
      "prepTimeMinutes": 20,
      "cookTimeMinutes": 15,
      "servings": 4,
      "difficulty": "Easy",
      "cuisine": "Italian",
      "caloriesPerServing": 300,
      "tags": [
        "Pizza",
        "Italian"
      ],
      "userId": 45,
      "image": "https://cdn.dummyjson.com/recipe-images/1.webp",
      "rating": 4.6,
      "reviewCount": 3,
      "mealType": [
        "Dinner"
      ]
    };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceipeDetail]
    }).compileComponents();

    fixture = TestBed.createComponent(ReceipeDetail);
    component = fixture.componentInstance;
    component.recipe = mockRecipe;
    fixture.detectChanges(); 
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render recipe details correctly', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Classic Margherita Pizza');
  });
});