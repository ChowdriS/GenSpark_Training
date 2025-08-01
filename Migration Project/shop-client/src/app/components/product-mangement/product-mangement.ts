import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product-service';
import { Product } from '../../model/Models';

@Component({
  selector: 'app-product-mangement',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-mangement.html',
  styleUrl: './product-mangement.css'
})
export class ProductMangement implements OnInit {
  productForm!: FormGroup;
  productList: Product[] = [];
  isEditing = false;
  editingId: number | null = null;
  originalProduct: any = null;

  constructor(private fb: FormBuilder, private productService: ProductService) {}

  ngOnInit() {
    this.initForm();
    this.loadProducts();
  }

  initForm() {
    const now = new Date().toISOString().slice(0, 16);
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      image: [''],
      price: [1, [Validators.required, Validators.min(0)]],
      userId: [1, Validators.required],
      categoryId: [null],
      colorId: [null],
      modelId: [null],
      storageId: [null],
      sellStartDate: [now, Validators.required],
      sellEndDate: [now, Validators.required],
      isNew: [1]
    });
  }

  loadProducts() {
    this.productService.getAll().subscribe({
      next: (data: any) => {
        this.productList = data.$values ?? data;
      },
      error: (err) => console.error('Load error:', err)
    });
  }

  onSubmit() {
    if (this.productForm.invalid) return;

    const formValue = {
      ...this.productForm.value,
      sellStartDate: new Date(this.productForm.value.sellStartDate).toISOString(),
      sellEndDate: new Date(this.productForm.value.sellEndDate).toISOString()
    };

    if (this.isEditing && this.editingId !== null && this.originalProduct) {
      const patch: any = {};
      for (const key in formValue) {
        const formVal = formValue[key];
        const originalVal = this.originalProduct[key];
        patch[key] =
          key.includes('Date') && originalVal
            ? new Date(formVal).toISOString() !== new Date(originalVal).toISOString()
              ? formVal
              : undefined
            : formVal !== originalVal
              ? formVal
              : undefined;
      }

      Object.keys(patch).forEach((k) => patch[k] === undefined && delete patch[k]);

      if (Object.keys(patch).length === 0) {
        console.log('No changes detected.');
        return;
      }

      this.productService.update(this.editingId, patch).subscribe({
        next: () => {
          this.resetForm();
          this.loadProducts();
        },
        error: (err) => console.error('Update failed:', err)
      });
    } else {
      this.productService.create(formValue).subscribe({
        next: () => {
          this.resetForm();
          this.loadProducts();
        },
        error: (err) => console.error('Create failed:', err)
      });
    }
  }

  editProduct(product: Product) {
    this.productForm.patchValue({
      productName: product.productName,
      image: product.image,
      price: product.price,
      userId: product.userId,
      categoryId: product.categoryId,
      colorId: product.colorId,
      modelId: product.modelId,
      storageId: product.storageId,
      sellStartDate: product.sellStartDate.slice(0, 16),
      sellEndDate: product.sellEndDate.slice(0, 16),
      isNew: product.isNew
    });
    this.originalProduct = { ...product };
    this.editingId = product.productId;
    this.isEditing = true;
  }

  deleteProduct(id: number) {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.delete(id).subscribe({
        next: () => this.loadProducts(),
        error: (err:any) => console.error('Delete failed:', err)
      });
    }
  }

  resetForm() {
    this.initForm();
    this.originalProduct = null;
    this.editingId = null;
    this.isEditing = false;
  }
}
