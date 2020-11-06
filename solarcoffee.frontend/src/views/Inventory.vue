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

@Component({
  name: 'Inventory',
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue {
  isNewProductVisible: boolean = false
  isShipmentVisible: boolean = false
  inventory: IProductInventory[] = [
    {
      id: 1,
      product: {
        id: 1,
        name: 'Dark Coffe',
        description: 'Dark hot coffee',
        price: 20,
        isTaxable: true,
        isArchived: false,
        createdOn: new Date(),
        updatedOn: new Date(),
      },
      quantityOnHand: 1,
      idealQuantity: 10,
    },
    {
      id: 2,
      product: {
        id: 2,
        name: 'Light Coffe',
        description: 'Light hot coffee',
        price: 25,
        isTaxable: false,
        isArchived: false,
        createdOn: new Date(),
        updatedOn: new Date(),
      },
      quantityOnHand: 2,
      idealQuantity: 10,
    },
    {
      id: 3,
      product: {
        id: 3,
        name: 'Light Small Coffe',
        description: 'Light hot coffee',
        price: 15,
        isTaxable: false,
        isArchived: false,
        createdOn: new Date(),
        updatedOn: new Date(),
      },
      quantityOnHand: 7,
      idealQuantity: 10,
    },
  ]

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

  saveNewShipment(newProduct: IProduct) {
    console.log(newProduct)
  }

  saveNewProduct(shipment: IShipment) {
    console.log(shipment)
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
