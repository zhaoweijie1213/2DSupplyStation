# 2DSupplyStation

您的新登录信息。我们系统不会记录这些信息，这些信息只会显示一次。

账号: 949210784@qq.com

密码: uvrUeeirk2ObXLB6tNDh

解压发布文件：

```shell
cd /home/docker/acg-supply-station
# 假设你的zip包叫 publish.zip
mkdir -p publish
unzip publish.zip -d publish
```

打包镜像：

```shell
cd /home/docker/acg-supply-station
docker buildx build --load -t acg-supply-station:v1.0.0 .
```

运行容器：

```shell
cd //home/docker/yml/ACGSupply
#运行docker容器
docker compose down
docker compose up -d
```

部署前端：

```
cd /home/docker/nginx
mkdir -p dist
unzip dist.zip -d dist
```



