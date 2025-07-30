import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorDelete } from './color-delete';

describe('ColorDelete', () => {
  let component: ColorDelete;
  let fixture: ComponentFixture<ColorDelete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorDelete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorDelete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
