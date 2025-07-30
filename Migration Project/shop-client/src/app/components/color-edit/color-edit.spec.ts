import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorEdit } from './color-edit';

describe('ColorEdit', () => {
  let component: ColorEdit;
  let fixture: ComponentFixture<ColorEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
