import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductMangement } from './product-mangement';

describe('ProductMangement', () => {
  let component: ProductMangement;
  let fixture: ComponentFixture<ProductMangement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductMangement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductMangement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
