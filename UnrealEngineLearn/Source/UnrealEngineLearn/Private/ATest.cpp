// Fill out your copyright notice in the Description page of Project Settings.


#include "ATest.h" 
#include "Kismet/GameplayStatics.h"

// Sets default values
AATest::AATest()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AATest::BeginPlay()
{
	Super::BeginPlay();
	FString string = TEXT("Hello");
	DebugLog(string);
}

// Called every frame
void AATest::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
	currentTime += DeltaTime;
	
	if (currentTime > timeInterval) {
		currentTime = 0;
		//UGameplayStatics::OpenLevel(GetWorld(), newLevel);
	}
}

void AATest::DebugLog(FString debugText)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.0f, FColor::Green, debugText);
}

