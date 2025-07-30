import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorList } from './color-list';

describe('ColorList', () => {
  let component: ColorList;
  let fixture: ComponentFixture<ColorList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
