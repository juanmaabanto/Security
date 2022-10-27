# Security API
Security API for request permissions

## Despliegue
Se requiere:
1. Docker, https://www.docker.com/products/docker-desktop/
2. Makefile, si se usa windows https://linuxhint.com/run-makefile-windows/

Una vez instalado ir a la raíz del proyecto y escribir en un terminal;

```sh
  make up

```

La descarga de las imagenes demora varios minutos(Principalmente por el Elastic Search)
Una vez se creen los contenedores esperar aprx. 3min para que cargue la instancia de SQL

Para ver el Swagger del servicio abrir el navegador e Ir a: http://localhost:5203/

## Elastic Search
Para ver el elastic search ir a: http://localhost:5601/app/dev_tools#/console
Luego ingresar la siguiente query
```sh
  GET _search
  {
    "query": {
      "query_string": {
        "default_field": "employeeSurname",
        "query": "*"
      }
    }
  }
```

## Capturas de la aplicación

### Contenedores

![alt text](https://github.com/juanmaabanto/Security/blob/main/docs/images/containers.png)

### Swagger

![alt text](https://github.com/juanmaabanto/Security/blob/main/docs/images/swagger.png)

### Kafka

![alt text](https://github.com/juanmaabanto/Security/blob/main/docs/images/kafka.png)

### Elastic Search

![alt text](https://github.com/juanmaabanto/Security/blob/main/docs/images/elastic_search.png)