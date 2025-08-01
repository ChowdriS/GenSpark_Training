import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaypallButton } from './paypall-button';

describe('PaypallButton', () => {
  let component: PaypallButton;
  let fixture: ComponentFixture<PaypallButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaypallButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaypallButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
