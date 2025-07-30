import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorCreate } from './color-create';

describe('ColorCreate', () => {
  let component: ColorCreate;
  let fixture: ComponentFixture<ColorCreate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorCreate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorCreate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
