#!/bin/bash
set -e

# 打印环境信息
env | grep -i branch

# 当前分支从 Jenkins 环境变量传入（比如 GIT_BRANCH）
BRANCH_NAME=$(echo "${GIT_BRANCH}" | sed 's|origin/||') # 去掉 origin/ 前缀（如果有）
echo "当前分支：$BRANCH_NAME"

# 获取 Git tag（确保完整）
BASE_VERSION=$(echo "$GIT_TAG" | sed 's/^v//')
FINAL_VERSION="$BASE_VERSION"
echo "🚀 master 构建版本号(来自参数 GIT_TAG): v$FINAL_VERSION"

# 构造最终的版本号
if [[ "$BRANCH_NAME" != "develop" && "$BRANCH_NAME" != "master" ]]; then
  FINAL_VERSION="${BASE_VERSION}.${BUILD_NUMBER}"
  echo "🔧 Test 构建版本号: v$FINAL_VERSION"
  docker buildx build --load -t ${SERVICE_NAME}:v${FINAL_VERSION} -f "${DOCKERFILE_PATH}" .
else
  FINAL_VERSION="${BASE_VERSION}"
  echo "🚀 正式构建版本号: v$FINAL_VERSION"
  docker buildx build --load -t ${SERVICE_NAME}:v${FINAL_VERSION} -f "${DOCKERFILE_PATH}" .
fi

# 写入 .env 文件（供 docker compose 使用）
echo "SERVICE_NAME=${SERVICE_NAME}" >"${WORKSPACE}/.env"
echo "IMAGE_VERSION=v${FINAL_VERSION}" >>"${WORKSPACE}/.env"
echo "BUILD_NUMBER=${BUILD_NUMBER}" >>"${WORKSPACE}/.env"
echo "BRANCH_NAME=${BRANCH_NAME}" >>"${WORKSPACE}/.env"
CLUSTER_VALUE="${Cluster:-default}"
echo "Cluster=${CLUSTER_VALUE}" >>"${WORKSPACE}/.env"

# 写入给 Jenkinsfile 使用的 build_meta.env
echo "IMAGE_TAG=v${FINAL_VERSION}" >"${WORKSPACE}/build_meta.env"

# 输出信息
echo "✅ 构建完成: ${SERVICE_NAME}:v${FINAL_VERSION}"
echo "✅ 当前 .env 内容如下："
cat "${WORKSPACE}/.env"
echo "✅ 当前 build_meta.env 内容如下："
cat "${WORKSPACE}/build_meta.env"
