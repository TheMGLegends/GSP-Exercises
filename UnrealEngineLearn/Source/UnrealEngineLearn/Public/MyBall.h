// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "MyBall.generated.h"

UCLASS()
class UNREALENGINELEARN_API AMyBall : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AMyBall();

	UPROPERTY(EditAnywhere, Category = "Spawner Category")
	TSubclassOf<AActor> ourSpawningObject;
	UPROPERTY(VisibleAnywhere, Category = "Spawner Category")
	APlayerController* playerController;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
	FVector RandomPos();
	void InstantiateBall();
};
