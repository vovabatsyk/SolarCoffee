<template>
  <div class="inventory-container">
    <h1 class="inventoryTitle">Inventory Dashboard</h1>
    <hr />
    <div class="inventory-actions">
      <solar-button @click.native="showNewProductModal" id="addNewBtn">
        Add New Item
      </solar-button>
      <solar-button @click.native="showShipmentModal" id="receiveShipmentBtn">
        Receive Shipment
      </solar-button>
    </div>
    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand</th>
        <th>Unit price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>{{ item.product.name }}</td>
        <td>{{ item.quantityOnHand }}</td>
        <td>{{ item.product.price | price }}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td><div>X</div></td>
      </tr>
    </table>

    <new-product-modal
      v-if="isNewProductVisible"
      @save:product="saveNewProduct"
      @close="closeModals"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      @save:shipment="saveNewShipment"
      :inventory="inventory"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import SolarButton from '@/components/SolarButton.vue'
import { IProduct, IProductInventory } from '@/interfaces/Product.d.ts'
import NewProductModal from '@/components/modals/NewProductModal.vue'
import ShipmentModal from '@/components/modals/ShipmentModal.vue'
import { IShipment } from '@/interfaces/Shipment'
import { InventoryService } from '@/services/inventory-service.ts'

const inventoryService = new InventoryService()

@Component({
  name: 'Inventory',
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue {
  isNewProductVisible: boolean = false
  isShipmentVisible: boolean = false
  inventory: IProductInventory[] = []

  showNewProductModal() {
    this.isNewProductVisible = true
  }

  showShipmentModal() {
    this.isShipmentVisible = true
  }

  closeModals() {
    this.isShipmentVisible = false
    this.isNewProductVisible = false
  }

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment)
    console.log(shipment)
    this.isShipmentVisible = false
    await this.initialize()
  }

  saveNewProduct(newProduct: IProduct) {
    console.log(newProduct)
  }
  async initialize() {
    this.inventory =  await inventoryService.getInventory()
  }

  async created() {
    await this.initialize()
  }
}
</script>

<style lang="scss" scoped>
.inventory-actions {
  display: flex;
  justify-content: flex-end;
}

.inventoryTitle {
  text-align: center;
}
</style>
