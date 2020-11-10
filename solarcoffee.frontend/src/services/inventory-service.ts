import { IShipment } from './../interfaces/Shipment.d'
import { IProductInventory } from './../interfaces/Product.d'
import axios from 'axios'

export class InventoryService {
  API_URL = process.env.VUE_APP_API_URL

  public async getInventory(): Promise<IProductInventory[]> {
    let result: any = await axios.get(`${this.API_URL}/inventory/`)
    return result.data
  }

  public async updateInventoryQuantity(shipment: IShipment) {
      let result: any = await axios.patch(`${this.API_URL}/inventory/`, shipment)
      return result.data 
  }
}
