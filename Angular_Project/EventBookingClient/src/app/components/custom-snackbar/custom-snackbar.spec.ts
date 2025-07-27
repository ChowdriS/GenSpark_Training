import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomSnackbarComponent } from './custom-snackbar';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { By } from '@angular/platform-browser';

describe('CustomSnackbarComponent', () => {
  let component: CustomSnackbarComponent;
  let fixture: ComponentFixture<CustomSnackbarComponent>;
  let mockSnackBarRef: jasmine.SpyObj<MatSnackBarRef<CustomSnackbarComponent>>;

  const snackBarData = {
    message: 'Test message',
    type: 'success' as 'success' | 'info' | 'error'
  };

  beforeEach(async () => {
    mockSnackBarRef = jasmine.createSpyObj('MatSnackBarRef', ['dismiss']);

    await TestBed.configureTestingModule({
      imports: [CustomSnackbarComponent, CommonModule],
      providers: [
        { provide: MatSnackBarRef, useValue: mockSnackBarRef },
        { provide: MAT_SNACK_BAR_DATA, useValue: snackBarData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CustomSnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render the message from MAT_SNACK_BAR_DATA', () => {
    const messageEl = fixture.debugElement.nativeElement.querySelector('span, div, p');    
    expect(messageEl.textContent).toContain(snackBarData.message);
  });

  it('should store injected data correctly', () => {
    expect(component.data).toEqual(snackBarData);
  });

  it('should call dismiss() when close() is called', () => {
    component.close();
    expect(mockSnackBarRef.dismiss).toHaveBeenCalled();
  });
});
