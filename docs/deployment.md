# Deployment Guide

## Overview

This guide covers deploying the e-commerce microservices platform to various environments, from development to production.

## Environment Strategy

### Development Environment
- **Purpose**: Local development and testing
- **Infrastructure**: Docker Compose
- **Database**: Local PostgreSQL, Redis, MongoDB
- **Authentication**: Local Keycloak instance

### Staging Environment
- **Purpose**: Pre-production testing
- **Infrastructure**: Kubernetes cluster
- **Database**: Managed cloud databases
- **Authentication**: Staging Keycloak realm

### Production Environment
- **Purpose**: Live application
- **Infrastructure**: Kubernetes cluster with high availability
- **Database**: Managed cloud databases with replication
- **Authentication**: Production Keycloak realm

## Prerequisites

### Required Tools
- **Docker**: For containerization
- **Kubernetes CLI (kubectl)**: For Kubernetes management
- **Helm**: For Kubernetes package management
- **Azure CLI** or **AWS CLI**: For cloud provider management

### Infrastructure Requirements
- **Kubernetes Cluster**: v1.24+ recommended
- **Container Registry**: Docker Hub, Azure Container Registry, or AWS ECR
- **Load Balancer**: For external traffic
- **Storage**: Persistent storage for databases
- **Monitoring**: Prometheus, Grafana, or cloud monitoring

## Docker Deployment

### 1. Build Docker Images

```bash
# Build all service images
docker build -t ecommerce/auth-service:latest src/Services/Auth/Auth.API/
docker build -t ecommerce/product-service:latest src/Services/Product/Product.API/
docker build -t ecommerce/cart-service:latest src/Services/Cart/Cart.API/
docker build -t ecommerce/order-service:latest src/Services/Order/Order.API/
docker build -t ecommerce/payment-service:latest src/Services/Payment/Payment.API/
docker build -t ecommerce/notification-service:latest src/Services/Notification/Notification.API/
docker build -t ecommerce/api-gateway:latest src/Gateway/API.Gateway/
docker build -t ecommerce/frontend:latest src/Web/ClientApp/
```

### 2. Push to Container Registry

```bash
# Tag images for your registry
docker tag ecommerce/auth-service:latest your-registry.azurecr.io/ecommerce/auth-service:latest
docker tag ecommerce/product-service:latest your-registry.azurecr.io/ecommerce/product-service:latest
# ... repeat for all services

# Push images
docker push your-registry.azurecr.io/ecommerce/auth-service:latest
docker push your-registry.azurecr.io/ecommerce/product-service:latest
# ... repeat for all services
```

### 3. Deploy with Docker Compose (Production)

```bash
# Create production docker-compose file
docker-compose -f docker-compose.prod.yml up -d
```

## Kubernetes Deployment

### 1. Create Namespace

```yaml
# k8s/namespace.yaml
apiVersion: v1
kind: Namespace
metadata:
  name: ecommerce
```

```bash
kubectl apply -f k8s/namespace.yaml
```

### 2. Create ConfigMaps and Secrets

```yaml
# k8s/config/auth-config.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: auth-config
  namespace: ecommerce
data:
  ConnectionStrings__DefaultConnection: "Host=postgres-auth;Port=5432;Database=auth_db;Username=auth_user;Password=auth_password"
  Jwt__Issuer: "https://api.ecommerce.com"
  Jwt__Audience: "https://ecommerce.com"
---
apiVersion: v1
kind: Secret
metadata:
  name: auth-secrets
  namespace: ecommerce
type: Opaque
data:
  Jwt__Key: <base64-encoded-jwt-key>
  Keycloak__ClientSecret: <base64-encoded-client-secret>
```

### 3. Deploy Databases

```yaml
# k8s/databases/postgres-auth.yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: postgres-auth
  namespace: ecommerce
spec:
  serviceName: postgres-auth
  replicas: 1
  selector:
    matchLabels:
      app: postgres-auth
  template:
    metadata:
      labels:
        app: postgres-auth
    spec:
      containers:
      - name: postgres
        image: postgres:15-alpine
        env:
        - name: POSTGRES_DB
          value: "auth_db"
        - name: POSTGRES_USER
          value: "auth_user"
        - name: POSTGRES_PASSWORD
          valueFrom:
            secretKeyRef:
              name: postgres-secrets
              key: password
        ports:
        - containerPort: 5432
        volumeMounts:
        - name: postgres-storage
          mountPath: /var/lib/postgresql/data
  volumeClaimTemplates:
  - metadata:
      name: postgres-storage
    spec:
      accessModes: ["ReadWriteOnce"]
      resources:
        requests:
          storage: 10Gi
---
apiVersion: v1
kind: Service
metadata:
  name: postgres-auth
  namespace: ecommerce
spec:
  selector:
    app: postgres-auth
  ports:
  - port: 5432
    targetPort: 5432
  type: ClusterIP
```

### 4. Deploy Services

```yaml
# k8s/services/auth-service.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: auth-service
  namespace: ecommerce
spec:
  replicas: 2
  selector:
    matchLabels:
      app: auth-service
  template:
    metadata:
      labels:
        app: auth-service
    spec:
      containers:
      - name: auth-service
        image: your-registry.azurecr.io/ecommerce/auth-service:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        envFrom:
        - configMapRef:
            name: auth-config
        - secretRef:
            name: auth-secrets
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: auth-service
  namespace: ecommerce
spec:
  selector:
    app: auth-service
  ports:
  - port: 80
    targetPort: 80
  type: ClusterIP
```

### 5. Deploy API Gateway

```yaml
# k8s/gateway/api-gateway.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-gateway
  namespace: ecommerce
spec:
  replicas: 3
  selector:
    matchLabels:
      app: api-gateway
  template:
    metadata:
      labels:
        app: api-gateway
    spec:
      containers:
      - name: api-gateway
        image: your-registry.azurecr.io/ecommerce/api-gateway:latest
        ports:
        - containerPort: 80
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
---
apiVersion: v1
kind: Service
metadata:
  name: api-gateway
  namespace: ecommerce
spec:
  selector:
    app: api-gateway
  ports:
  - port: 80
    targetPort: 80
  type: LoadBalancer
```

### 6. Deploy Frontend

```yaml
# k8s/frontend/frontend.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: frontend
  namespace: ecommerce
spec:
  replicas: 3
  selector:
    matchLabels:
      app: frontend
  template:
    metadata:
      labels:
        app: frontend
    spec:
      containers:
      - name: frontend
        image: your-registry.azurecr.io/ecommerce/frontend:latest
        ports:
        - containerPort: 80
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
---
apiVersion: v1
kind: Service
metadata:
  name: frontend
  namespace: ecommerce
spec:
  selector:
    app: frontend
  ports:
  - port: 80
    targetPort: 80
  type: LoadBalancer
```

## Cloud-Specific Deployment

### Azure Kubernetes Service (AKS)

#### 1. Create AKS Cluster
```bash
# Create resource group
az group create --name ecommerce-rg --location eastus

# Create AKS cluster
az aks create \
  --resource-group ecommerce-rg \
  --name ecommerce-cluster \
  --node-count 3 \
  --enable-addons monitoring \
  --generate-ssh-keys

# Get credentials
az aks get-credentials --resource-group ecommerce-rg --name ecommerce-cluster
```

#### 2. Deploy Azure Container Registry
```bash
# Create ACR
az acr create --resource-group ecommerce-rg --name ecommerceacr --sku Basic

# Build and push images
az acr build --registry ecommerceacr --image auth-service:latest src/Services/Auth/Auth.API/
az acr build --registry ecommerceacr --image product-service:latest src/Services/Product/Product.API/
# ... repeat for all services
```

#### 3. Deploy to AKS
```bash
# Apply Kubernetes manifests
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/config/
kubectl apply -f k8s/databases/
kubectl apply -f k8s/services/
kubectl apply -f k8s/gateway/
kubectl apply -f k8s/frontend/
```

### AWS Elastic Kubernetes Service (EKS)

#### 1. Create EKS Cluster
```bash
# Create cluster
eksctl create cluster \
  --name ecommerce-cluster \
  --region us-east-1 \
  --nodegroup-name standard-workers \
  --node-type t3.medium \
  --nodes 3 \
  --nodes-min 1 \
  --nodes-max 4
```

#### 2. Deploy to EKS
```bash
# Update image references to ECR
# Apply Kubernetes manifests
kubectl apply -f k8s/
```

## CI/CD Pipeline

### GitHub Actions

```yaml
# .github/workflows/deploy.yml
name: Deploy to Production

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Set up .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Build and push Docker images
      run: |
        docker build -t ${{ secrets.REGISTRY }}/auth-service:${{ github.sha }} src/Services/Auth/Auth.API/
        docker build -t ${{ secrets.REGISTRY }}/product-service:${{ github.sha }} src/Services/Product/Product.API/
        # ... build all services
        
        docker push ${{ secrets.REGISTRY }}/auth-service:${{ github.sha }}
        docker push ${{ secrets.REGISTRY }}/product-service:${{ github.sha }}
        # ... push all services
    
    - name: Deploy to Kubernetes
      run: |
        kubectl set image deployment/auth-service auth-service=${{ secrets.REGISTRY }}/auth-service:${{ github.sha }}
        kubectl set image deployment/product-service product-service=${{ secrets.REGISTRY }}/product-service:${{ github.sha }}
        # ... update all services
```

## Monitoring and Observability

### 1. Health Checks
```yaml
# Add to service deployments
livenessProbe:
  httpGet:
    path: /health
    port: 80
  initialDelaySeconds: 30
  periodSeconds: 10
readinessProbe:
  httpGet:
    path: /health/ready
    port: 80
  initialDelaySeconds: 5
  periodSeconds: 5
```

### 2. Logging
```yaml
# Configure logging sidecar or use cloud logging
- name: logging-sidecar
  image: fluent/fluent-bit
  volumeMounts:
  - name: varlog
    mountPath: /var/log
  - name: varlibdockercontainers
    mountPath: /var/lib/docker/containers
    readOnly: true
```

### 3. Metrics
```yaml
# Add Prometheus annotations
metadata:
  annotations:
    prometheus.io/scrape: "true"
    prometheus.io/port: "80"
    prometheus.io/path: "/metrics"
```

## Security Considerations

### 1. Network Policies
```yaml
# k8s/network-policies/default-deny.yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: default-deny
  namespace: ecommerce
spec:
  podSelector: {}
  policyTypes:
  - Ingress
  - Egress
```

### 2. RBAC
```yaml
# k8s/rbac/service-account.yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: auth-service-account
  namespace: ecommerce
---
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  namespace: ecommerce
  name: auth-service-role
rules:
- apiGroups: [""]
  resources: ["pods"]
  verbs: ["get", "list"]
```

### 3. Secrets Management
- Use **Azure Key Vault** or **AWS Secrets Manager**
- Implement **secret rotation**
- Use **RBAC** for secret access
- Encrypt **secrets at rest**

## Scaling and Performance

### 1. Horizontal Pod Autoscaling
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: auth-service-hpa
  namespace: ecommerce
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: auth-service
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

### 2. Resource Limits
```yaml
resources:
  requests:
    memory: "256Mi"
    cpu: "250m"
  limits:
    memory: "512Mi"
    cpu: "500m"
```

## Backup and Disaster Recovery

### 1. Database Backups
```bash
# Automated PostgreSQL backups
kubectl create job --from=cronjob/postgres-backup postgres-backup-manual
```

### 2. Application Data
- **Persistent Volumes**: Use cloud storage
- **Regular Snapshots**: Automated backup schedules
- **Cross-Region Replication**: For disaster recovery

### 3. Configuration Management
- **GitOps**: Use ArgoCD or Flux
- **Configuration Versioning**: Track all changes
- **Rollback Procedures**: Quick recovery processes

## Troubleshooting

### 1. Common Issues
```bash
# Check pod status
kubectl get pods -n ecommerce

# Check pod logs
kubectl logs -f deployment/auth-service -n ecommerce

# Check service endpoints
kubectl get endpoints -n ecommerce

# Check events
kubectl get events -n ecommerce --sort-by='.lastTimestamp'
```

### 2. Performance Issues
```bash
# Check resource usage
kubectl top pods -n ecommerce

# Check node resources
kubectl top nodes

# Check network policies
kubectl get networkpolicies -n ecommerce
```

### 3. Security Issues
```bash
# Check RBAC
kubectl auth can-i get pods --as=system:serviceaccount:ecommerce:auth-service-account

# Check secrets
kubectl get secrets -n ecommerce

# Check service accounts
kubectl get serviceaccounts -n ecommerce
``` 