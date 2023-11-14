// Fill out your copyright notice in the Description page of Project Settings.


#include "MyBall.h"
#include <Kismet/GameplayStatics.h>

// Sets default values
AMyBall::AMyBall()
{
	PlayerController = nullptr;
	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	//MyComp = FindComponentByClass<UStaticMeshComponent>();
}

// Called when the game starts or when spawned
void AMyBall::BeginPlay()
{
	Super::BeginPlay();
	PlayerController = UGameplayStatics::GetPlayerController(GetWorld(), 0);
	//MyComp->OnComponentWake.AddDynamic(this, &AMyBall::OnMyComponentWake);
}

// Called every frame
void AMyBall::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);

	if (PlayerController->WasInputKeyJustPressed(EKeys::B)) {
		InstantiateObject(10, BallObject);
	}
	else if (PlayerController->WasInputKeyJustPressed(EKeys::C)) {
		InstantiateObject(10, CubeObject);
	}
}

FVector AMyBall::RandomPos()
{
	return FVector(FMath::RandRange(-1000, 1000), FMath::RandRange(-1000, 1000), FMath::RandRange(-1000, 1000));
}

void AMyBall::InstantiateObject(const int AmountToSpawn, const TSubclassOf<AActor> Object)
{
	for (size_t i = 0; i < AmountToSpawn; i++)
	{
		FActorSpawnParameters SpawnParams;
		SpawnParams.Owner = this;
		SpawnParams.Instigator = GetInstigator();
		//AActor* OurNewObject = GetWorld()->SpawnActor<AActor>(object, RandomPos(), FRotator(0), spawnParams);
		GetWorld()->SpawnActor<AActor>(Object, RandomPos(), FRotator(0), SpawnParams);
	}
}

void AMyBall::OnMyComponentWake(UPrimitiveComponent* WakingComponent, FName BoneName)
{
	DebugLog("Ow!");
}

void AMyBall::DebugLog(FString debugText)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.0f, FColor::Green, debugText);
}

