import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReceipeList } from './receipe-list';
import { ReceipeService } from '../../services/receipe-service';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ReceipeDetail } from '../receipe-detail/receipe-detail';

describe('ReceipeList', () => {
  let component: ReceipeList;
  let fixture: ComponentFixture<ReceipeList>;
  let mockReceipeService: jasmine.SpyObj<ReceipeService>;

  const mockRecipes = [
    { id: 1, name: 'Classic Margherita Pizza' },
    { id: 2, name: 'Normal Pizza' }
  ];

  beforeEach(async () => {
    const receipeServiceSpy = jasmine.createSpyObj('ReceipeService', ['getAllReceipe']);

    await TestBed.configureTestingModule({
      imports: [ReceipeList, CommonModule, ReceipeDetail],
      providers: [
        { provide: ReceipeService, useValue: receipeServiceSpy }
      ]
    }).compileComponents();

    mockReceipeService = TestBed.inject(ReceipeService) as jasmine.SpyObj<ReceipeService>;
    mockReceipeService.getAllReceipe.and.returnValue(of({ recipes: mockRecipes }));

    fixture = TestBed.createComponent(ReceipeList);
    component = fixture.componentInstance;
    fixture.detectChanges(); 
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call getAllReceipe and update recipes signal', () => {
    expect(mockReceipeService.getAllReceipe).toHaveBeenCalled();
    expect(component.recipes()).toEqual(mockRecipes);
    expect(component.loading()).toBeFalse();
  });
});
