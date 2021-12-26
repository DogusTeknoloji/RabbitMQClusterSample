#!/bin/zsh

docker-compose up -d

sleep 60

docker exec rabbitmqtest_rabbitmq2_1 rabbitmqctl stop_app
docker exec rabbitmqtest_rabbitmq2_1 rabbitmqctl reset
docker exec rabbitmqtest_rabbitmq2_1 rabbitmqctl join_cluster rabbit@rabbitmq1
docker exec rabbitmqtest_rabbitmq2_1 rabbitmqctl start_app

docker exec rabbitmqtest_rabbitmq3_1 rabbitmqctl stop_app
docker exec rabbitmqtest_rabbitmq3_1 rabbitmqctl reset
docker exec rabbitmqtest_rabbitmq3_1 rabbitmqctl join_cluster rabbit@rabbitmq2
docker exec rabbitmqtest_rabbitmq3_1 rabbitmqctl start_app

docker exec rabbitmqtest_rabbitmq4_1 rabbitmqctl stop_app
docker exec rabbitmqtest_rabbitmq4_1 rabbitmqctl reset
docker exec rabbitmqtest_rabbitmq4_1 rabbitmqctl join_cluster rabbit@rabbitmq3
docker exec rabbitmqtest_rabbitmq4_1 rabbitmqctl start_app

docker exec rabbitmqtest_rabbitmq1_1 rabbitmqctl cluster_status
docker exec rabbitmqtest_rabbitmq2_1 rabbitmqctl cluster_status
docker exec rabbitmqtest_rabbitmq3_1 rabbitmqctl cluster_status
docker exec rabbitmqtest_rabbitmq4_1 rabbitmqctl cluster_status

