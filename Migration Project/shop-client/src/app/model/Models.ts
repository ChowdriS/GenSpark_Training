
export interface Cart {
  product?: Product;
  quantity: number;
}

export interface Category {
  categoryId: number;
  name?: string;
  products?: Product[];
}

export interface Color {
  colorId: number;
  color1?: string;
  products?: Product[];
}

export interface ContactU {
  id: number;
  name?: string;
  email?: string;
  phone?: string;
  content?: string;
}

export interface News {
  newsId: number;
  userId?: number;
  title?: string;
  shortDescription?: string;
  image?: string;
  content?: string;
  createdDate: string;   
  status?: number;
  user?: User;
}

export interface Order {
  orderID: number;
  orderName?: string;
  orderDate: string;     
  paymentType?: string;
  status?: string;
  customerName?: string;
  customerPhone?: string;
  customerEmail?: string;
  customerAddress?: string;
  orderDetails?: OrderDetail[];
}

export interface OrderDetail {
  orderID: number;
  productID: number;
  price?: number;
  quantity?: number;
  order?: Order;
  product?: Product;
}

export interface Product {
  productId: number;
  productName?: string;
  image?: string;
  price?: number;
  userId?: number;
  categoryId?: number;
  colorId?: number;
  modelId?: number;
  storageId?: number;
  sellStartDate: string;  
  sellEndDate: string;    
  isNew?: number;
  category?: Category;
  color?: Color;
  model?: Model;
  orderDetails?: OrderDetail[];
  user?: User;
}

export interface User {
  userId: number;
  username?: string;
  password?: string;
  news?: News[];
  products?: Product[];
}

export interface Model {
  ModelId:number;
    Model1:string;
}
