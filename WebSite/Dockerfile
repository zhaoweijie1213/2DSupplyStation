FROM nginx:1.26 as base
EXPOSE 80

COPY /_nginx/nginx.conf /etc/nginx/nginx.conf
COPY /_nginx/env.js /etc/nginx/env.js
COPY /_nginx/default.conf /etc/nginx/conf.d/default.conf
COPY /dist/ /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
