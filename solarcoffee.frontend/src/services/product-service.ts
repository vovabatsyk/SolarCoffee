import { IProduct } from './../interfaces/Product.d'
import axios from 'axios'

export class ProductService {
    API_URL = process.env.VUE_APP_API_URL

    async archive(productId: number) {
        let result = await axios.patch(`${this.API_URL}/product/${productId}`)
        return result.data
    }

    async save(newProduct: IProduct) {
        let result = await axios.post(`${this.API_URL}/product/`, newProduct)
        return result.data    
    }
}