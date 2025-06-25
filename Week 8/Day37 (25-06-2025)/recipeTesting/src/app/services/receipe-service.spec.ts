import { TestBed } from '@angular/core/testing';
import {  HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ReceipeService } from './receipe-service';
import { provideHttpClient } from '@angular/common/http';

describe('RecipeService', () => {
  let service: ReceipeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [ReceipeService, provideHttpClient(), provideHttpClientTesting()]
    });

    service = TestBed.inject(ReceipeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should fetch recipes', () => {
    const mockData = {
      recipes: [
        {
          id: 1,
          name: "Classic Margherita Pizza"
        },
        ...Array.from({ length: 29 }, (_, i) => ({
          id: i + 2,
          name: `Item ${i + 2}`
        }))
      ]
    };

    service.getAllReceipe().subscribe((response: any) => {
      expect(response.recipes.length).toBe(30);  
      expect(response.recipes[0].name).toBe('Classic Margherita Pizza'); 
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipes');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });

  afterEach(() => httpMock.verify());
});
