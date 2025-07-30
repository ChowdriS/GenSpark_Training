// DTOs & Requests

export interface CategoryRequestDTO {
  categoryName?: string;
}

export interface ColorRequestDTO {
  colorName?: string;
}

export interface ContactRequestDTO {
  name?: string;
  email?: string;
  phone?: string;
  content?: string;
  captchaToken?: string;
}

export interface NewsRequestDTO {
  userId: number;
  title?: string;
  shortDescription?: string;
  image?: string;
  content?: string;
  createdDate: string;   
  status: number;
}

export interface NewsUpdateRequestDTO {
  title?: string;
  shortDescription?: string;
  image?: string;
  content?: string;
  status?: number;
}

export interface OrderRequestDTO {
  orderName?: string;
  orderDate: string;     
  paymentType?: string;
  status?: string;
  customerName?: string;
  customerPhone?: string;
  customerEmail?: string;
  customerAddress?: string;
}

export interface OrderUpdateRequestDTO {
  status?: string;
  customerName?: string;
  customerPhone?: string;
  customerEmail?: string;
  customerAddress?: string;
}

export interface PaginatedResult<T> {
  page: number;
  pageSize: number;
  totalItems: number;
  items: T[];
}

export interface ProductRequestDTO {
  productName?: string;
  description?: string;
  categoryId: number;
  price: number;
  quantity: number;
}
