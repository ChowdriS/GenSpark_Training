import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsMangement } from './news-mangement';

describe('NewsMangement', () => {
  let component: NewsMangement;
  let fixture: ComponentFixture<NewsMangement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewsMangement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewsMangement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
