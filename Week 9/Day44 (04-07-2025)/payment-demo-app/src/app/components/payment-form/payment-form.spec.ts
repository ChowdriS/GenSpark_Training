import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentForm } from './payment-form';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('PaymentForm', () => {
  let component: PaymentForm;
  let fixture: ComponentFixture<PaymentForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentForm,ReactiveFormsModule,CommonModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  afterEach(() => {
    delete (window as any).Razorpay;
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should validate required fields', () => {
    const { amount, customerName, email, contact } = component.form.controls;

    amount.setValue(null);
    customerName.setValue('');
    email.setValue('');
    contact.setValue('');

    expect(component.form.invalid).toBeTrue();

    amount.setValue(100);
    customerName.setValue('user');
    email.setValue('user@example.com');
    contact.setValue('9876543210');

    expect(component.form.valid).toBeTrue();
  });

  it('should not call Razorpay if form is invalid', () => {
    const razorpaySpy = jasmine.createSpy('Razorpay');
    (window as any).Razorpay = razorpaySpy;

    component.form.patchValue({ amount: null });
    component.payNow();

    expect(razorpaySpy).not.toHaveBeenCalled();
  });

  it('should call Razorpay.open() if form is valid', () => {
    const openSpy = jasmine.createSpy('open');
    const razorpayMock = jasmine.createSpy().and.returnValue({ open: openSpy });
    (window as any).Razorpay = razorpayMock;
    component.form.setValue({
      amount: 100,
      customerName: 'user',
      email: 'user@example.com',
      contact: '9876543210',
    });
    component.payNow();
    expect(razorpayMock).toHaveBeenCalled();
    expect(openSpy).toHaveBeenCalled();
  });
});
