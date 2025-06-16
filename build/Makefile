# Makefile pour automatiser les tâches courantes

.PHONY: help build test clean docker

# Affiche l'aide
help:
	@echo "Usage:"
	@echo "  make build     - Compile le projet"
	@echo "  make test      - Lance tous les tests"
	@echo "  make clean     - Nettoie les artifacts"
	@echo "  make docker    - Build l'image Docker"
	@echo "  make up        - Lance tous les conteneurs"
	@echo "  make down      - Arrête tous les conteneurs"

# Build le projet
build:
	dotnet build

# Lance les tests
test:
	dotnet test

# Tests unitaires uniquement
test-unit:
	dotnet test --filter "Category=Unit"

# Tests d'intégration uniquement
test-int:
	dotnet test --filter "Category=Integration"

# Tests E2E uniquement
test-e2e:
	dotnet test --filter "Category=E2E"

# Nettoie les artifacts
clean:
	dotnet clean
	find . -type d -name bin -exec rm -rf {} +
	find . -type d -name obj -exec rm -rf {} +

# Build l'image Docker
docker:
	docker build -t scparchive-api .

# Lance les conteneurs
up:
	docker-compose up -d

# Arrête les conteneurs
down:
	docker-compose down
