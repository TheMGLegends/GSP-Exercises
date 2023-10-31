// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ATest.generated.h"

UCLASS()
class UNREALENGINELEARN_API AATest : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AATest();

	UPROPERTY(VisibleAnywhere, Category = "Level Changing Settings")
	float currentTime;
	UPROPERTY(EditAnywhere, Category = "Level Changing Settings")
	float timeInterval;
	UPROPERTY(EditAnywhere, Category = "Level Changing Settings")
	FName newLevel;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	UFUNCTION(BlueprintCallable, Category = "Debug Category")
	void DebugLog(FString debugText);

};
