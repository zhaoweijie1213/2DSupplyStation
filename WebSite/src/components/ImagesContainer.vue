<template>
  <v-container fluid>
    <v-infinite-scroll
      v-if="images?.length > 0"
      class="img_container"
      :items="images"
      :onLoad="load"
    >
      <v-row dense>
        <v-col
          v-for="(image, index) in images"
          :key="index"
          cols="6"
          sm="4"
          md="4"
          lg="3"
          xl="2"
          xxl="2"
        >
          <v-card>
            <img :src="image.filePath" class="align-end img-custom" cover />
            <v-card-title class="text-white text-center app-card-title"
              >编号：{{ image.fileName }}</v-card-title
            >
          </v-card>
        </v-col>
      </v-row>
    </v-infinite-scroll>
    <v-empty-state
      v-else
      icon="mdi-magnify"
      text="Try adjusting your search terms or filters. Sometimes less specific terms or broader queries can help you find what you're looking for."
      title="We couldn't find a match."
    ></v-empty-state>
  </v-container>
</template>
<script setup lang="ts">
import { ref, watch } from "vue";
import { useRoute } from "vue-router";
import { ImageInfo, ImagesServiceProxy } from "@/api/api";

const props = defineProps<{
  product: string;
  hidden: boolean;
}>();

const images = ref<ImageInfo[]>([]);
const route = useRoute();
const auth: string = route.query.auth as string;
var pageNum = 1;
const pageSize = 10;

// 监听 product 的变化，重新获取图片
watch(
  () => props.product,
  async (newProduct, oldProduct) => {
    if (newProduct !== oldProduct) {
      pageNum = 1; // 重置分页参数，如果有分页需求
      images.value = [];
      const res = await getImages(newProduct);
      if (res.code == 0 && res.data) {
        images.value.push(...res.data);
      }
    }
  },
  { immediate: true } // 组件加载时立即执行一次
);
async function getImages(name: string) {
  var client = new ImagesServiceProxy();
  var res;
  if (props.hidden) {
    res = await client.hid(auth, pageNum, pageSize);
    return res;
  } else {
    res = await client.list(name, auth, pageNum, pageSize);
  }
  return res;
}

async function load({ done }: any) {
  done("loading");
  // Perform API call
  const res = await getImages(props.product);
  if (res.code == 0 && res.data) {
    if (res.data.length > 0) {
      images.value.push(...res.data);
      done("ok");
    } else {
      done("empty");
    }
  } else {
    done("error");
  }
}
</script>
<style scoped>
.img-custom {
  width: 100%;
}
.app-card-title {
  font-size: 0.75rem;
}
</style>
