import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceipesComponent } from './receipes.component';

describe('ReceipesComponent', () => {
  let component: ReceipesComponent;
  let fixture: ComponentFixture<ReceipesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceipesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReceipesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
