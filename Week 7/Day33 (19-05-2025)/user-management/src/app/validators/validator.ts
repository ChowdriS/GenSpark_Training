import { AbstractControl, ValidationErrors } from '@angular/forms';

const words = ['admin', 'root'];

export function UsernameValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value?.toLowerCase();
  if (value && words.some((word) => value.includes(word))) {
    return { InvalidUsername: true };
  }
  return null;
}

export function matchPasswords(group: AbstractControl): ValidationErrors | null {
    const pass = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { passwordsMismatch: true };
  }