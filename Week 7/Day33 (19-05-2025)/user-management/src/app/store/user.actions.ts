import { createAction, props } from "@ngrx/store";
import { User } from "../model/usermodel";


export const addUser = createAction('[Users] Add User',props<{ user: User }>());
export const getAllUser = createAction('[Users] Get All Users');