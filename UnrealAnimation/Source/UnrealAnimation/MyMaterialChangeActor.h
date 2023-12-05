// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"

#include "Materials/Material.h"
#include "Components/MeshComponent.h"
#include "Materials/MaterialInstanceDynamic.h"

#include "NiagaraComponent.h"

#include "MyMaterialChangeActor.generated.h"


UCLASS()
class UNREALANIMATION_API AMyMaterialChangeActor : public AActor
{
	GENERATED_BODY()
	
public:	
	UMaterialInstanceDynamic* MyDynamicMat;

	UPROPERTY(EditAnywhere)
	UMaterialInstance* MyMat;

	UPROPERTY(EditAnywhere)
	UMaterial* MyMat2;

	// Sets default values for this actor's properties
	AMyMaterialChangeActor();

	const float time = 5;
	float currentTime = 5;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
