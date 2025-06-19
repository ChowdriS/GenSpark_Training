import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectUserState = createFeatureSelector<any>('user');

export const selectAllUsers = createSelector(selectUserState,state=> state.users);