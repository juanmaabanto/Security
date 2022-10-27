FROM mcr.microsoft.com/mssql/server:2022-latest

RUN mkdir -p /usr/work
WORKDIR /usr/work

COPY . /usr/work/

USER root
RUN chmod +x /usr/work/import.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh