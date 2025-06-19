import { Component } from '@angular/core';
import { ReactiveFormsModule,FormBuilder,Validators } from '@angular/forms';
import { matchPasswords, UsernameValidator } from '../../validators/validator';
import { Observable } from 'rxjs';
import { User } from '../../model/usermodel';
import { Store } from '@ngrx/store';
import { selectAllUsers } from '../../store/user.selector';
import { addUser } from '../../store/user.actions';

@Component({
  selector: 'app-user-form',
  imports: [ReactiveFormsModule],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css'
})
export class UserForm {
  roles = ['Admin', 'User', 'Guest'];

  form: ReturnType<FormBuilder['group']>;

  constructor(private store:Store,private fb: FormBuilder) {
    this.form = this.fb.group(
      {
        username: ['', [Validators.required, UsernameValidator]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        role: ['', Validators.required],
      },
      { validators: [matchPasswords] }
    );
  }
  get f() {
    return this.form.controls;
  }

  submit() {
    if (this.form.valid) {
      // let user : User = {username : "chowdri"};
      // this.store.dispatch(addUser({ user: user}));
      // console.log(this.form.value)
      let newUser: User = {username : this.form.value.username}
      alert('User Created');
      this.store.dispatch(addUser({ user: newUser}));
      this.form.reset();
    }
  }
}
