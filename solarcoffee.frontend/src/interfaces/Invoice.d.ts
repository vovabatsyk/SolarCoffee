import { IProduct } from './Product.d'
export interface IInvoice {
  customerId: number
  lineItems: ILineItems[]
  createdOn: Date
  updatedOn: Date
}

export interface ILineItems {
  product?: IProduct
  quantity: number
}
