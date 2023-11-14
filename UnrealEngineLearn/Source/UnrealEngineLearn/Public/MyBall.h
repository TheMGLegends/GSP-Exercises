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
	TSubclassOf<AActor> BallObject;
	UPROPERTY(EditAnywhere, Category = "Spawner Category")
	TSubclassOf<AActor> CubeObject;
	UPROPERTY(VisibleAnywhere, Category = "Spawner Category")
	APlayerController* PlayerController;
	UPROPERTY(VisibleAnywhere, Category = "Sure")
	UStaticMeshComponent* MyComp;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
	static FVector RandomPos();
	void InstantiateObject(int AmountToSpawn, TSubclassOf<AActor> Object);
	UFUNCTION()
	void OnMyComponentWake(class UPrimitiveComponent* WakingComponent, FName BoneName);
	UFUNCTION(BlueprintCallable, Category = "Debug Category")
	void DebugLog(FString debugText);
};
