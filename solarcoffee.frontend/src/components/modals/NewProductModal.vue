<template>
  <solar-modal>
    <template v-slot:header>
      Add New Product
    </template>
    <template v-slot:body>
      <ul class="newProduct">
        <li class="taxable">
          <label for="isTaxable">Is this product taxable?</label>
          <input
            type="checkbox"
            v-model="newProduct.isTaxable"
            id="isTaxable"
          />
        </li>
        <li>
          <label for="productName">Name</label>
          <input type="text" v-model="newProduct.name" id="productName" />
        </li>
        <li>
          <label for="productDesc">Description</label>
          <input
            type="text"
            v-model="newProduct.description"
            id="productDesc"
          />
        </li>
        <li>
          <label for="productPrice">Price (USD)</label>
          <input type="number" v-model="newProduct.price" id="productPrice" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button
        type="button"
        @click.native="save"
        aria-label="Save New Item"
      >
        Save Product
      </solar-button>
      <solar-button
        type="button"
        @click.native="close"
        aria-label="Close Modal"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import SolarButton from '@/components/SolarButton.vue'
import SolarModal from '@/components/modals/SolarModal.vue'
import { IProduct, IProductInventory } from '@/interfaces/Product'

@Component({
  name: 'NewProductModal',
  components: { SolarButton, SolarModal },
})
export default class NewProductModal extends Vue {
  newProduct: IProduct = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    name: '',
    description: '',
    price: 0,
    isTaxable: true,
    isArchived: false,
  }

  close() {
    this.$emit('close')
  }

  save() {
    if(this.newProduct.name != "" && this.newProduct.price > 0){
      this.$emit('save:product', this.newProduct)
    }
  }
}
</script>

<style lang="scss" scoped>
.newProduct {
  list-style: none;
  padding: 0;
  margin: 0;

  .taxable {
    display: flex;

    input {
        width: 10%;
    }

    label {
        margin-top: 0.5rem;
    }
  }
}
</style>
