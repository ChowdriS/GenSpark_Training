import { Component, computed, OnInit, signal } from '@angular/core';
import { BehaviorSubject, debounceTime, distinctUntilChanged, Observable } from 'rxjs';
import { User } from '../../model/usermodel';
import { Store } from '@ngrx/store';
import { selectAllUsers } from '../../store/user.selector';
import { addUser, getAllUser } from '../../store/user.actions';

@Component({
  selector: 'app-search-user',
  imports: [],
  templateUrl: './search-user.html',
  styleUrl: './search-user.css'
})
export class SearchUser implements OnInit{
  searchTerm = signal('');
  searchInput$ = new BehaviorSubject<string>("");

  users = signal<User[]>([]);

  constructor(private store:Store) {
    this.searchInput$
      .pipe(debounceTime(1000))
      .subscribe((term) => this.searchTerm.set(term));
    this.store.select(selectAllUsers).subscribe((user)=>{
      this.users.set(user || [])
    });
  }

  onSearch(term: Event) {
    const input = term.target as HTMLInputElement;
    const value = input.value;
    this.searchTerm.set(value);
  }

  ngOnInit() {
    this.store.dispatch(getAllUser());    
  }

  filterUsers = computed(() => {
    const term = this.searchTerm().toLowerCase();
    return this.users().filter(user =>
      user.username.toLowerCase().includes(term)
    );
  });
}
