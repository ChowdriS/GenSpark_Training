import { createReducer, on } from "@ngrx/store";
import { addUser, getAllUser } from "./user.actions";
import { User } from "../model/usermodel";

const initialState:any = {
  users: [
    { username: 'apple' },
    { username: 'banana' }
  ]
};

export const UserReducer = createReducer(
    initialState,
    on(getAllUser, (state)=>({...state})),
    on(addUser, (state,{user})=>({...state, users: [...state.users, user]})),
)